export interface NoteDTO {
  id: number;
  title: string;
  content: unknown[];
  userId: string;
  createdAt: string;
  updatedAt: string;
}

export interface CreateNoteDTO {
  title: string;
  content: unknown[];
  userId: number;
}
