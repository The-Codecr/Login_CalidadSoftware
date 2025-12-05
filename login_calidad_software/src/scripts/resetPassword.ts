import { toast } from "sonner";
import { validatePassword } from "@src/utils/passwordValidator";

export function setupResetPasswordForm() {
  const form = document.getElementById("newPasswordForm") as HTMLFormElement | null;

  if (!form) {
    console.error("No se encontró el formulario de restablecimiento de contraseña.");
    return;
  }

  form.addEventListener("submit", async (e) => {
    e.preventDefault();

    const pass = (document.getElementById("password") as HTMLInputElement)?.value;
    const pass2 = (document.getElementById("password2") as HTMLInputElement)?.value;
    const email = (document.getElementById("email") as HTMLInputElement)?.value;
    const token = (document.getElementById("token") as HTMLInputElement)?.value;

    if (!pass || !pass2 || !email || !token) {
      toast.error("Complete todos los campos.");
      return;
    }

    if (pass !== pass2) {
      toast.error("Las contraseñas no coinciden.");
      return;
    }

    // 🔐 VALIDAR CONTRASEÑA
    const validation = validatePassword(pass);
    if (!validation.isValid) {
      const errorMessage = validation.errors.join(", ");
      toast.error(`Contraseña inválida: ${errorMessage}`);
      return;
    }

    try {
      const APP_URL = import.meta.env.PUBLIC_API_URL;

      const res = await fetch(`${APP_URL}/forgotPassword`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, token, password: pass }),
      });

      const data = await res.json();

      if (data.success) {
        toast.success("Contraseña actualizada correctamente.");

        setTimeout(() => {
          window.location.href = "/login";
        }, 1500);
      } else {
        toast.error(data.message || "Error al actualizar contraseña.");
      }
    } catch (error) {
      console.error("Error al conectar con el servidor:", error);
      toast.error("Error de conexión con el servidor.");
    }
  });
}