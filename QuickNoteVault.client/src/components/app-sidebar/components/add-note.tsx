import { createNote } from '@/api/notes/resources';
import { Button } from '@/components/ui/button';
import { useMutation } from '@tanstack/react-query';
import { PlusIcon } from 'lucide-react';
import { useNavigate } from 'react-router';

export function AddNote() {
  const navigate = useNavigate();
  const { mutate } = useMutation({
    mutationFn: () => {
      return createNote({
        title: 'Untitled Note',
        content: [{
          type: 'p',
          children: [
            {
              text: '',
            },
          ],
        }],
        userId: 1,
      });
    },
    onSuccess: (result) => {
      navigate(`/notes/${result}`);
    },
  });

  return (
    <Button
      onClick={() => {
        mutate();
      }}
      className="flex w-full group-data-[collapsible=icon]:!size-8 group-data-[collapsible=icon]:!p-2 [&>span:last-child]:truncate [&>svg]:size-4 [&>svg]:shrink-0 transition-[width,height,padding] overflow-hidden"
      size="sm"
    >
      <PlusIcon />
      <span className="group-data-[collapsible=icon]:hidden">
        Add Note
      </span>
    </Button>
  );
}
