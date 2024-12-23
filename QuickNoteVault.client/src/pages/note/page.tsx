import type { NoteFormValues } from '@/lib/types/notes/note-form-values';
import type { Value } from '@udecode/plate-common';
import type { loader } from './loader';
import { updateNote } from '@/api/notes/resources';
import { PlateEditor } from '@/components/editor/plate-editor';
import { Page, PageContent, PageHeader, PageHeaderContent, PageSidebarTrigger } from '@/components/page';
import { Form, FormControl, FormField, FormItem, FormLabel } from '@/components/ui/form';
import { Input } from '@/components/ui/input';
import { useMutation } from '@tanstack/react-query';
import { useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { useLoaderData } from 'react-router';
import { useDebounceCallback } from 'usehooks-ts';
import { DeleteNote } from './components/delete-note';

export default function Note() {
  const data = useLoaderData<typeof loader>();

  const form = useForm<NoteFormValues>({
    defaultValues: {
      title: data.title,
      content: data.content,
    },
  });

  const { mutate: update } = useMutation({
    mutationFn: updateNote,
  });

  const debouncedUpdate = useDebounceCallback(update, 600);

  const { watch } = form;

  useEffect(() => {
    const subscription = watch((values) => {
      debouncedUpdate({
        id: data.id,
        title: values.title,
        content: values.content as Value,
        userId: 1,
      });
    });

    return () => {
      subscription.unsubscribe();
    };
  }, [watch, debouncedUpdate, data.id]);

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
