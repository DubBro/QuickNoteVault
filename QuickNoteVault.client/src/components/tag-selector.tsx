import { cn } from '@/lib/utils';
import { CheckIcon, PlusIcon, X } from 'lucide-react';
import { useState } from 'react';
import { Button } from './ui/button';
import { Command, CommandEmpty, CommandGroup, CommandInput, CommandItem, CommandList } from './ui/command';
import { Popover, PopoverContent, PopoverTrigger } from './ui/popover';

interface Tag {
  id: string;
  name: string;
}

export interface TagSelectorProps {
  title?: string;
  tags?: Tag[];
  selectedTabs?: string[];
  onTagSelect?: (tag: string) => void;
  onTagCreate?: (tag: string) => void;
}

const emptyArray: any[] = [];
function noop() {}

function TagSelector({ title, tags = emptyArray, selectedTabs = emptyArray, onTagSelect = noop, onTagCreate = noop }: TagSelectorProps) {
  const [open, setOpen] = useState(false);
  const [searchTerm, setSearchTerm] = useState('');
  const shownTags = tags.filter(tag => selectedTabs.includes(tag.id));

  const handleSelect = (tag: string) => {
    onTagSelect(tag);
    setOpen(false);
    setSearchTerm('');
  };

  const handleCreateTag = () => {
    onTagCreate(searchTerm);
    setOpen(false);
    setSearchTerm('');
  };

  return (
    <div className="flex gap-1">
      {shownTags.map(tag => (
        <Button key={tag.id} variant="secondary" size="sm" onClick={() => onTagSelect(tag.id)}>
          {tag.name}
          <X />
        </Button>
      ))}
      <Popover open={open} onOpenChange={setOpen}>
        <PopoverTrigger asChild>
          <Button variant="ghost" size="sm">
            <PlusIcon />
            {title}
          </Button>
        </PopoverTrigger>
        <PopoverContent align="start" className="p-0 w-auto">
          <Command>
            <CommandInput placeholder={title} value={searchTerm} onValueChange={setSearchTerm} />
            <CommandList>
              {searchTerm !== ''
                ? (
                    <CommandEmpty className="py-2">
                      <Button variant="ghost" size="sm" onClick={handleCreateTag}>
                        <span className="font-normal">
                          Add
                        </span>
                        {searchTerm}
                      </Button>
                    </CommandEmpty>
                  )
                : (
                    <CommandEmpty>No tags found.</CommandEmpty>
                  )}
              {tags.length > 0
                ? (
                    <CommandGroup>
                      {tags.map(tag => (
                        <CommandItem key={tag.id} value={tag.id} onSelect={handleSelect}>
                          {tag.name}
                          <CheckIcon className={cn('ml-auto', selectedTabs.includes(tag.id) ? 'opacity-100' : 'opacity-0')} />
                        </CommandItem>
                      ))}
                    </CommandGroup>
                  )
                : null}
            </CommandList>
          </Command>
        </PopoverContent>
      </Popover>
    </div>
  );
}

export default TagSelector;
