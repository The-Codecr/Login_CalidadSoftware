// login_calidad_software/src/utils/passwordValidator.ts

export interface PasswordValidation {
  isValid: boolean;
  errors: string[];
}

export const validatePassword = (password: string): PasswordValidation => {
  const errors: string[] = [];

  // Validar longitud mínima
  if (password.length < 5) {
    errors.push("Mínimo 5 caracteres");
  }

  // Validar longitud máxima
  if (password.length > 10) {
    errors.push("Máximo 10 caracteres");
  }

  // Validar al menos una mayúscula
  if (!/[A-Z]/.test(password)) {
    errors.push("Debe contener al menos una mayúscula");
  }

  // Validar al menos un carácter especial
  if (!/[!@#$%^&*(),.?":{}|<>_\-+=\[\]\\\/`~;]/.test(password)) {
    errors.push("Debe contener al menos un carácter especial (!@#$%^&*...)");
  }

  return {
    isValid: errors.length === 0,
    errors
  };
};

export const getPasswordRequirements = (): string => {
  return "La contraseña debe tener entre 5 y 10 caracteres, al menos una mayúscula y un carácter especial (!@#$%^&*...)";
};