import type { CreateNoteDTO, NoteDTO } from './types';

export async function getAllNotes(): Promise<NoteDTO[]> {
  const response = await fetch(`${import.meta.env.VITE_API_URL}/note/getall/1`);

  return response.json();
}

export async function getNoteById(noteId: number): Promise<NoteDTO> {
  const response = await fetch(`${import.meta.env.VITE_API_URL}/note/get/${noteId}`);

  if (!response.ok) {
    throw response.json();
  }

  return response.json();
}

export async function createNote(note: CreateNoteDTO): Promise<number> {
  const response = await fetch(`${import.meta.env.VITE_API_URL}/note/add`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(note),
  });

  return response.json();
}

export async function updateNote(note: NoteDTO): Promise<NoteDTO> {
  const response = await fetch(`${import.meta.env.VITE_API_URL}/note/update`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(note),
  });

  return response.json();
}

export async function deleteNoteById(noteId: number) {
  const response = await fetch(`${import.meta.env.VITE_API_URL}/note/delete/${noteId}`, {
    method: 'DELETE',
  });

  return response.ok;
}
