import { Page, PageContent, PageHeader, PageHeaderContent, PageSidebarTrigger } from '@/components/page';

function Home() {
  return (
    <Page>
      <PageHeader>
        <PageHeaderContent>
          <PageSidebarTrigger />
        </PageHeaderContent>
      </PageHeader>
      <PageContent>Home</PageContent>
    </Page>
  );
}

export default Home;
