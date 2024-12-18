import { createBrowserRouter } from 'react-router';
import AddNote from './pages/add-note/page';
import Home from './pages/home/page';
import Root, { ErrorBoundary } from './root';

export const router = createBrowserRouter([
  {
    path: '/',
    element: <Root />,
    errorElement: <ErrorBoundary />,
    children: [
      {
        index: true,
        element: <Home />,
      },
      {
        path: '/notes',
        children: [
          {
            path: 'add',
            element: <AddNote />,
          },
        ],
      },
    ],
  },
]);
