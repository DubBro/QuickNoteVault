import { useQuery } from '@tanstack/react-query';
import { getAllNotes, getNoteById } from './resources';

export function getNotesQueryKey() {
  return ['notes'];
}

export function useNotesQuery() {
  return useQuery({
    queryKey: ['notes'],
    queryFn: getAllNotes,
  });
}

export function getNoteQueryKey(id: number) {
  return ['notes', id];
}

export function useNoteQuery(id: number) {
  return useQuery({
    queryKey: getNoteQueryKey(id),
    queryFn: () => getNoteById(id),
    enabled: Boolean(id),
  });
}
