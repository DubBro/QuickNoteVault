import { Button } from './ui/button';
import { DialogClose, DialogContent, DialogDescription, DialogFooter, DialogTitle } from './ui/dialog';

interface RemoveNoteDialogProps {
  onRemove: () => void;
}

export function RemoveNoteDialog({ onRemove }: RemoveNoteDialogProps) {
  return (
    <DialogContent>
      <DialogTitle>Delete note</DialogTitle>
      <DialogDescription>
        Are you sure you want to delete this note? This action cannot be undone.
      </DialogDescription>
      <DialogFooter>
        <DialogClose asChild>
          <Button variant="ghost" size="sm">Cancel</Button>
        </DialogClose>
        <Button
          variant="destructive"
          size="sm"
          onClick={onRemove}
        >
          Delete
        </Button>
      </DialogFooter>
    </DialogContent>
  );
}
