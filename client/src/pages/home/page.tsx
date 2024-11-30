import { Page, PageContent, PageHeader, PageHeaderContent, PageSidebarTrigger } from '@/components/page';
import { NoteFilters } from './components/note-filters';

export default function Home() {
  return (
    <Page>
      <PageHeader>
        <PageHeaderContent>
          <PageSidebarTrigger />
        </PageHeaderContent>
      </PageHeader>
      <PageContent>
        <NoteFilters />
      </PageContent>
    </Page>
  );
}
