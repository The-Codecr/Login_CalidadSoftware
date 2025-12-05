
import { sendEmail } from "@libs";
import { toast } from "sonner";

export function setupForgotPasswordForm() {
  const form = document.getElementById("resetForm") as HTMLFormElement | null;
  const emailInput = document.getElementById("email") as HTMLInputElement | null;
  const errorSpan = document.getElementById("mensajeError") as HTMLSpanElement | null;

  if (!form || !emailInput || !errorSpan) {
    console.error("No se encontró el formulario de reset password.");
    return;
  }

  form.addEventListener("submit", async (e) => {
    e.preventDefault();

    const email = emailInput.value.trim();

    if (!email || !email.includes("@")) {
      errorSpan.textContent = "Ingrese un correo válido.";
      return;
    }

    errorSpan.textContent = "";

    const APP_URL = import.meta.env.PUBLIC_RES_URL;

    const resetLink = `${APP_URL}/resetPassword?email=${encodeURIComponent(email)}&token=${crypto.randomUUID()}`;

    try {
      const result = await sendEmail({
        user_email: email,
        reset_link: resetLink
      });

      if (result.success) {
        toast.success("Se envió el enlace de restablecimiento a tu correo.");
        form.reset();
      } else {
        toast.error("Error enviando correo. Intente más tarde.");
      }
    } catch (err) {
      console.error("Error enviando correo:", err);
      toast.error("No se pudo enviar el correo.");
    }
  });
}
