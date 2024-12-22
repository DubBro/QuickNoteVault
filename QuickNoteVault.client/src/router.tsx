import { createBrowserRouter } from 'react-router';
import Home from './pages/home/page';
import NewNote from './pages/new-note/page';
import { loader as noteLoader } from './pages/note/loader';
import Note from './pages/note/page';
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
            path: 'new',
            element: <NewNote />,
          },
          {
            path: ':id',
            loader: noteLoader,
            element: <Note />,
          },
        ],
      },
    ],
  },
]);
