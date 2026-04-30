using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace AtividadeExtensionistaFaculdadeBackend.Services;

public sealed class EmailService(IConfiguration configuration, ILogger<EmailService> logger)
{
    public async Task SendAsync(string toEmail, string toName, string subject, string htmlBody, CancellationToken ct)
    {
        var emailSettings = configuration.GetSection("Email");

        var senderEmail = ResolveEnvVar(emailSettings["SenderEmail"] ?? string.Empty);
        var password = ResolveEnvVar(emailSettings["Password"] ?? string.Empty);

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(
            emailSettings["SenderName"] ?? "My Smart Money",
            senderEmail));
        message.To.Add(new MailboxAddress(toName, toEmail));
        message.Subject = subject;

        message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = htmlBody
        };

        using var client = new SmtpClient();

        await client.ConnectAsync(
            emailSettings["SmtpHost"] ?? "smtp.gmail.com",
            int.Parse(emailSettings["SmtpPort"] ?? "587"),
            SecureSocketOptions.StartTls,
            ct);

        await client.AuthenticateAsync(senderEmail, password, ct);

        await client.SendAsync(message, ct);
        await client.DisconnectAsync(quit: true, ct);

        logger.LogInformation("Email sent to {Email} with subject '{Subject}'", toEmail, subject);
    }

    public async Task SendEmailConfirmationAsync(string toEmail, string toName, string confirmationUrl, CancellationToken ct)
    {
        var subject = "Confirme seu e-mail — My Smart Money";
        var html = $"""
            <h2>Bem-vindo ao My Smart Money, {toName}!</h2>
            <p>Clique no botão abaixo para confirmar seu e-mail e ativar sua conta:</p>
            <a href="{confirmationUrl}"
               style="display:inline-block;padding:12px 24px;background:#2E5EA8;color:#fff;text-decoration:none;border-radius:6px;font-size:16px;">
               Confirmar e-mail
            </a>
            <p style="color:#6B7280;margin-top:16px;">Se você não criou uma conta, ignore este e-mail.</p>
            """;

        await SendAsync(toEmail, toName, subject, html, ct);
    }

    public async Task SendTwoFactorCodeAsync(string toEmail, string toName, string code, CancellationToken ct)
    {
        var subject = "Seu código de acesso — My Smart Money";
        var html = $"""
            <h2>Código de verificação</h2>
            <p>Use o código abaixo para concluir seu login. Ele expira em <strong>10 minutos</strong>.</p>
            <div style="font-size:36px;font-weight:bold;letter-spacing:8px;color:#1B2A4A;padding:16px;background:#F5F7FA;border-radius:8px;text-align:center;">
                {code}
            </div>
            <p style="color:#6B7280;margin-top:16px;">Se você não tentou fazer login, ignore este e-mail.</p>
            """;

        await SendAsync(toEmail, toName, subject, html, ct);
    }

    public async Task SendPasswordResetAsync(string toEmail, string toName, string resetUrl, CancellationToken ct)
    {
        var subject = "Redefinição de senha — My Smart Money";
        var html = $"""
            <h2>Redefinir senha</h2>
            <p>Recebemos uma solicitação para redefinir sua senha. Clique no botão abaixo:</p>
            <a href="{resetUrl}"
               style="display:inline-block;padding:12px 24px;background:#2E5EA8;color:#fff;text-decoration:none;border-radius:6px;font-size:16px;">
               Redefinir minha senha
            </a>
            <p style="color:#6B7280;margin-top:16px;">Este link expira em <strong>1 hora</strong>. Se você não solicitou a redefinição, ignore este e-mail.</p>
            """;

        await SendAsync(toEmail, toName, subject, html, ct);
    }

    // Expands %VAR_NAME% placeholders to their environment variable values,
    // matching the same convention used for the database connection string.
    private static string ResolveEnvVar(string value)
    {
        if (!value.StartsWith('%') || !value.EndsWith('%') || value.Length < 3)
            return value;

        var varName = value[1..^1];
        return Environment.GetEnvironmentVariable(varName) ?? value;
    }
}
