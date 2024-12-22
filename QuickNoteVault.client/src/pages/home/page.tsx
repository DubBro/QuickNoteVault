import { useNotesQuery } from '@/api/notes/queries';
import { Page, PageContent, PageHeader, PageHeaderContent, PageSidebarTrigger } from '@/components/page';
import { NoteCard, NoteCardSkeleton } from './components/note-card';
import { NoteFilters } from './components/note-filters';

export default function Home() {
  const { data, isLoading } = useNotesQuery();

  return (
    <Page>
      <PageHeader>
        <PageHeaderContent>
          <PageSidebarTrigger />
        </PageHeaderContent>
      </PageHeader>
      <PageContent>
        <NoteFilters />
        <div className="flex gap-4 flex-wrap">
          {isLoading
            ? (
                Array.from({ length: 8 }).map((_, i) => (
                  // eslint-disable-next-line react/no-array-index-key
                  <NoteCardSkeleton key={i} className="w-1/5 h-48" />
                ))
              )
            : data?.map(note => (
              <NoteCard
                key={note.id}
                id={note.id}
                title={note.title}
                updatedAt={note.modifiedAt}
                className="w-1/5 h-48"
              />
            ))}
        </div>
      </PageContent>
    </Page>
  );
}
