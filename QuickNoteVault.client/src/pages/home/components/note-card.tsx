import { Skeleton } from '@/components/ui/skeleton';
import { cn } from '@/lib/utils';
import { Link } from 'react-router';

const dateFormatter = new Intl.DateTimeFormat('uk', {
  dateStyle: 'short',
});

interface NoteCardProps {
  id: number;
  title: string;
  updatedAt: string;
  className?: string;
}

export function NoteCard({
  id,
  updatedAt,
  title,
  className,
}: NoteCardProps) {
  return (
    <Link to={`/notes/${id}`} className={cn('p-4 flex items-center justify-center border rounded-lg hover:bg-muted transition-colors', className)}>
      <div className="flex flex-col items-center justify-center gap-2">
        <h3 className="font-semibold text-center text-xl leading-tight">{title}</h3>
        <span className="text-slate-500 font-medium text-sm tabular-nums">{dateFormatter.format(new Date(updatedAt))}</span>
      </div>
    </Link>
  );
}

export function NoteCardSkeleton({ ...props }: React.ComponentPropsWithRef<'div'>) {
  return (
    <div {...props} className={cn('p-4 flex items-center justify-center border rounded-lg', props.className)}>
      <div className="flex flex-col items-center w-full justify-center gap-2">
        <Skeleton className="h-6 w-1/2" />
        <Skeleton className="h-4 w-1/4" />
      </div>
    </div>
  );
}
