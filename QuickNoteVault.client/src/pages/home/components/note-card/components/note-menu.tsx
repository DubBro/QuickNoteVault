import { getNotesQueryKey } from '@/api/notes/queries';
import { deleteNoteById } from '@/api/notes/resources';
import { RemoveNoteDialog } from '@/components/remove-note-dialog';
import { Button } from '@/components/ui/button';
import { Dialog, DialogTrigger } from '@/components/ui/dialog';
import { DropdownMenu, DropdownMenuContent, DropdownMenuItem, DropdownMenuTrigger } from '@/components/ui/dropdown-menu';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { MoreVerticalIcon } from 'lucide-react';
import { useNavigate } from 'react-router';

interface NoteMenuProps {
  noteId: number;
}

export function NoteMenu({ noteId }: NoteMenuProps) {
  const removeNote = useRemoveNote();

  return (
    <Dialog>
      <DropdownMenu>
        <DropdownMenuTrigger asChild>
          <Button
            variant="secondary"
            onClick={(e) => {
              e.preventDefault();
            }}
            size="icon"
            className="absolute top-2 right-2"
          >
            <MoreVerticalIcon />
          </Button>
        </DropdownMenuTrigger>
        <DropdownMenuContent align="end" onClick={e => e.stopPropagation()}>
          <DialogTrigger asChild>
            <DropdownMenuItem>Remove</DropdownMenuItem>
          </DialogTrigger>
        </DropdownMenuContent>
      </DropdownMenu>
      <RemoveNoteDialog onRemove={() => {
        removeNote(noteId);
      }}
      />
    </Dialog>
  );
}

function useRemoveNote() {
  const navigate = useNavigate();
  const queryClient = useQueryClient();

  const { mutate } = useMutation({
    mutationFn: deleteNoteById,
    onSuccess: () => {
      navigate('/');
      queryClient.invalidateQueries({
        queryKey: getNotesQueryKey(),
      });
    },
  });

  return mutate;
}
