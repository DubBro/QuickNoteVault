import type { LoaderFunctionArgs } from 'react-router';
import { noteQueryKey } from '@/api/notes/queries';
import { getNoteById } from '@/api/notes/resources';
import { queryClient } from '@/api/query-client';
import { redirect } from 'react-router';

export async function loader({ params }: LoaderFunctionArgs) {
  try {
    const data = await queryClient.fetchQuery({
      queryKey: noteQueryKey(Number(params.id)),
      queryFn: () => getNoteById(Number(params.id)),
    });

    return data;
  }
  catch {
    throw redirect('/');
  }
}
