import type { NoteFormValues } from '@/lib/types/notes/note-form-values';
import { PlateEditor } from '@/components/editor/plate-editor';
import { Page, PageContent, PageHeader, PageHeaderContent, PageSidebarTrigger } from '@/components/page';
import { Form, FormControl, FormField, FormItem, FormLabel } from '@/components/ui/form';
import { Input } from '@/components/ui/input';
import { useForm } from 'react-hook-form';

// interface Tag {
//   id: string;
//   name: string;
// }

export default function NewNote() {
  // const [tags, setTags] = useState<Tag[]>([]);
  // const [selectedTags, setSelectedTags] = useState<string[]>([]);

  // useEffect(() => {
  //   setTags([
  //     {
  //       id: '1',
  //       name: 'Personal',
  //     },
  //     {
  //       id: '2',
  //       name: 'Work',
  //     },
  //     {
  //       id: '3',
  //       name: 'Important',
  //     },
  //   ]);
  // }, []);

  // const handleTagSelect = (tag: string) => {
  //   if (selectedTags.includes(tag)) {
  //     setSelectedTags(prevTags => prevTags.filter(t => t !== tag));
  //   }
  //   else {
  //     setSelectedTags(prevTags => [...prevTags, tag]);
  //   }
  // };

  // const handleTagCreate = (tag: string) => {
  //   const newTag = { id: String(tags.length + 1), name: tag };
  //   setTags(prevTags => [...prevTags, newTag]);
  //   setSelectedTags(prevTags => [...prevTags, newTag.id]);
  // };

  const form = useForm<NoteFormValues>({
    defaultValues: {
      title: '',
      content: [{
        type: 'p',
        children: [
          {
            text: '',
          },
        ],
      }],
    },
  });

  return (
    <Page>
      <PageHeader>
        <PageHeaderContent>
          <PageSidebarTrigger />
        </PageHeaderContent>
      </PageHeader>
      <PageContent className="space-y-4 pb-4">
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
