import type { NoteFormValues } from '@/lib/types/notes/note-form-values';
import type { loader } from './loader';
import { PlateEditor } from '@/components/editor/plate-editor';
import { Page, PageContent, PageHeader, PageHeaderContent, PageSidebarTrigger } from '@/components/page';
import { Form, FormControl, FormField, FormItem, FormLabel } from '@/components/ui/form';
import { Input } from '@/components/ui/input';
import { useForm } from 'react-hook-form';
import { useLoaderData } from 'react-router';
import { DeleteNote } from './components/delete-note';

export default function Note() {
  const data = useLoaderData<typeof loader>();

  const form = useForm<NoteFormValues>({
    defaultValues: {
      title: data.title,
      content: data.content,
    },
  });

  return (
    <Page>
      <PageHeader>
        <PageHeaderContent className="justify-between">
          <PageSidebarTrigger />
          <DeleteNote />
        </PageHeaderContent>
      </PageHeader>
      <PageContent className="space-y-4">
        <Form {...form}>
          <FormField
            control={form.control}
            name="title"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Note name</FormLabel>
                <FormControl>
                  <Input placeholder="Enter a note name" className="max-w-96" {...field} />
                </FormControl>
              </FormItem>
            )}
          />
          <FormField
            control={form.control}
            name="content"
            render={({ field }) => (
              <div className="grid border rounded-lg">
                <PlateEditor
                  value={field.value}
                  onValueChange={field.onChange}
                />
              </div>
            )}
          />
        </Form>
      </PageContent>
    </Page>
  );
}
