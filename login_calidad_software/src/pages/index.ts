export function GET({ request }: { request: Request }) {
    const url = new URL('/login', request.url);
    return Response.redirect(url.toString(), 302);
  }
  