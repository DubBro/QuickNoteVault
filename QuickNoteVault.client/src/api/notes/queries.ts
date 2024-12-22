import { useQuery } from '@tanstack/react-query';
import { getAllNotes, getNoteById } from './resources';

export function useNotesQuery() {
  return useQuery({
    queryKey: ['notes'],
    queryFn: getAllNotes,
  });
}

export function noteQueryKey(id: number) {
  return ['notes', id];
}

export function useNoteQuery(id: number) {
  return useQuery({
    queryKey: noteQueryKey(id),
    queryFn: () => getNoteById(id),
    enabled: Boolean(id),
  });
}
