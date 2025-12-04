
import emailjs from "@emailjs/browser";

const SERVICE_ID = import.meta.env.PUBLIC_EMAILJS_SERVICE_ID;
const TEMPLATE_ID = import.meta.env.PUBLIC_EMAILJS_TEMPLATE_ID;
const PUBLIC_KEY  = import.meta.env.PUBLIC_EMAILJS_PUBLIC_KEY;

export interface EmailData {
    user_email: string;
    reset_link: string;
  }
  
export async function sendEmail(data: EmailData) {
  try {
    const response = await emailjs.send(
      SERVICE_ID,
      TEMPLATE_ID,
      data as unknown as Record<string, unknown>, 
      PUBLIC_KEY
    );

    return {
      success: true,
      status: response.status,
      text: response.text
    };
  } catch (error) {
    console.error("EmailJS Error:", error);

    return {
      success: false,
      error
    };
  }
}
