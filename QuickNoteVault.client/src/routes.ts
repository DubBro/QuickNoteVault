import { index, prefix, route, type RouteConfig } from '@react-router/dev/routes';

export default [
  index('./pages/home/page.tsx'),
  ...prefix('/notes', [
    route('/add', './pages/add-note/page.tsx'),
  ]),

] satisfies RouteConfig;
