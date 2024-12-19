import {
  isRouteErrorResponse,
  Outlet,
  useRouteError,
} from 'react-router';
import AppSidebar from './components/app-sidebar';
import { SidebarProvider } from './components/ui/sidebar';
import './index.css';

export default function Root() {
  return (
    <SidebarProvider>
      <AppSidebar />
      <Outlet />
    </SidebarProvider>
  );
}

export function ErrorBoundary() {
  const error = useRouteError();

  let message = 'Oops!';
  let details = 'An unexpected error occurred.';
  let stack: string | undefined;

  if (isRouteErrorResponse(error)) {
    message = error.status === 404 ? '404' : 'Error';
    details
      = error.status === 404
        ? 'The requested page could not be found.'
        : error.statusText || details;
  }
  else if (import.meta.env.DEV && error && error instanceof Error) {
    details = error.message;
    stack = error.stack;
  }

  return (
    <main className="pt-16 p-4 container mx-auto">
      <h1>{message}</h1>
      <p>{details}</p>
      {stack && (
        <pre className="w-full p-4 overflow-x-auto">
          <code>{stack}</code>
        </pre>
      )}
    </main>
  );
}
