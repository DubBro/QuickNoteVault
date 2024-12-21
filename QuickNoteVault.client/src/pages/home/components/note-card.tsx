import { cn } from '@/lib/utils';
import { Link } from 'react-router';

const dateFormatter = new Intl.DateTimeFormat('uk', {
  dateStyle: 'short',
});

interface NoteCardProps {
  id: number;
  title: string;
  createdAt: string;
  className?: string;
}

export function NoteCard({
  id,
  createdAt,
  title,
  className,
}: NoteCardProps) {
  return (
    <Link to={`/notes/${id}`} className={cn('p-4 flex items-center justify-center border rounded-lg hover:bg-muted transition-colors', className)}>
      <div className="flex flex-col items-center justify-center gap-2">
        <h3 className="font-semibold text-center text-xl leading-tight">{title}</h3>
        <span className="text-slate-500 font-medium text-sm tabular-nums">{dateFormatter.format(new Date(createdAt))}</span>
      </div>
    </Link>
  );
}
