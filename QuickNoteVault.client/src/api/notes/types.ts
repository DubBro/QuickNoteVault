export interface NoteDTO {
  id: number;
  title: string;
  content: any[];
  userId: string;
  createdAt: string;
  modifiedAt: string;
}

export interface CreateNoteDTO {
  title: string;
  content: any[];
  userId: number;
}

export interface UpdateNoteDTO {
  id: number;
  title: string;
  content: any[];
  userId: number;
}
