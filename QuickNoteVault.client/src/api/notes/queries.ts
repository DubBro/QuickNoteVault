import { useQuery } from '@tanstack/react-query';
import { getAllNotes } from './resources';

export function useNotesQuery() {
  return useQuery({
    queryKey: ['notes'],
    queryFn: getAllNotes,
  });
}
