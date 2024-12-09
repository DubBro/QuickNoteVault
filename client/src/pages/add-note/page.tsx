import { Page, PageContent, PageHeader, PageHeaderContent, PageSidebarTrigger } from '@/components/page';
import TagSelector from '@/components/tag-selector';
import { Input } from '@/components/ui/input';
import { Textarea } from '@/components/ui/textarea';
import { useEffect, useState } from 'react';

interface Tag {
  id: string;
  name: string;
}

export default function AddNote() {
  const [tags, setTags] = useState<Tag[]>([]);
  const [selectedTags, setSelectedTags] = useState<string[]>([]);

  useEffect(() => {
    setTags([
      {
        id: '1',
        name: 'Personal',
      },
      {
        id: '2',
        name: 'Work',
      },
      {
        id: '3',
        name: 'Important',
      },
    ]);
  }, []);

  const handleTagSelect = (tag: string) => {
    if (selectedTags.includes(tag)) {
      setSelectedTags(prevTags => prevTags.filter(t => t !== tag));
    }
    else {
      setSelectedTags(prevTags => [...prevTags, tag]);
    }
  };

  const handleTagCreate = (tag: string) => {
    const newTag = { id: String(tags.length + 1), name: tag };
    setTags(prevTags => [...prevTags, newTag]);
    setSelectedTags(prevTags => [...prevTags, newTag.id]);
  };

  return (
    <Page>
      <PageHeader>
        <PageHeaderContent>
          <PageSidebarTrigger />
        </PageHeaderContent>
      </PageHeader>
      <PageContent className="px-4">
        <div className="py-4">
          <Input placeholder="Enter a Note name" />
        </div>
        <div className="grid grid-cols-[auto_1fr] items-center gap-x-4">
          <span className="font-medium text-sm">Tags</span>
          <TagSelector title="Tag" tags={tags} selectedTabs={selectedTags} onTagSelect={handleTagSelect} onTagCreate={handleTagCreate} />
          <span className="font-medium text-sm">Collaborators</span>
          <TagSelector title="Collaborator" />
        </div>
        <div className="py-4">
          <Textarea placeholder="Enter a Note description" />
        </div>
      </PageContent>
    </Page>
  );
}
