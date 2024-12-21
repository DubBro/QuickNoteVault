import { useNotesQuery } from '@/api/notes/queries';
import { Page, PageContent, PageHeader, PageHeaderContent, PageSidebarTrigger } from '@/components/page';
import { NoteCard } from './components/note-card';
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
        <div className="flex px-4 gap-4 flex-wrap">
          {isLoading
            ? (
                <p>Loading...</p>
              )
            : data?.map(note => (
              <NoteCard
                key={note.id}
                id={note.id}
                title={note.title}
                createdAt={note.createdAt}
                className="w-1/4 h-48"
              />
            ))}
        </div>
      </PageContent>
    </Page>
  );
}
