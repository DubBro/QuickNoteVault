import { getNotesQueryKey } from '@/api/notes/queries';
import { deleteNoteById } from '@/api/notes/resources';
import { RemoveNoteDialog } from '@/components/remove-note-dialog';
import { Button } from '@/components/ui/button';
import { Dialog, DialogTrigger } from '@/components/ui/dialog';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { Trash2Icon } from 'lucide-react';
import { useNavigate, useParams } from 'react-router';

export function DeleteNote() {
  const params = useParams<{ id: string }>();
  const navigate = useNavigate();

  const queryClient = useQueryClient();
  const { mutate } = useMutation({
    mutationFn: async () => {
      if (!params.id)
        return;

      await deleteNoteById(Number(params.id));
    },
    onSuccess: () => {
      navigate('/');
      queryClient.invalidateQueries({
        queryKey: getNotesQueryKey(),
      });
    },
  });

  return (
    <Dialog>
      <DialogTrigger asChild>
        <Button variant="ghost" className="hover:bg-destructive/10 hover:text-destructive">
          <Trash2Icon />
          Delete
        </Button>
      </DialogTrigger>
      <RemoveNoteDialog onRemove={mutate} />
    </Dialog>

  );
}
