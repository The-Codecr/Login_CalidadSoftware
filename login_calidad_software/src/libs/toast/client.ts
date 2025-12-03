import { toastAdapter } from "./adapter";
import type { ToasterProps } from "sonner";

export const toastConfig: {
  position: ToasterProps["position"];
  defaultDuration: number;
  richColors: boolean;
} = {
  position: "top-right",
  defaultDuration: 3000,
  richColors: true,
};


export const notify = {
  success: (msg: string) => toastAdapter.success(msg),
  error: (msg: string) => toastAdapter.error(msg),
  info: (msg: string) => toastAdapter.info(msg),
  warning: (msg: string) => toastAdapter.warning(msg),
  loading: (msg: string) => toastAdapter.loading(msg),
  promise: <T>(promise: Promise<T>, msgs: any) =>
    toastAdapter.promise(promise, msgs),
};
