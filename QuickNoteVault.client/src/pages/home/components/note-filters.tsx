import { Button } from '@/components/ui/button';
import { Command, CommandEmpty, CommandGroup, CommandInput, CommandList } from '@/components/ui/command';
import { Input } from '@/components/ui/input';
import { Popover, PopoverContent, PopoverTrigger } from '@/components/ui/popover';
import { PlusCircle, Settings2Icon } from 'lucide-react';

interface NoteFilterProps {
  title: string;
}

function NoteFilter({ title }: NoteFilterProps) {
  return (
    <Popover>
      <PopoverTrigger asChild>
        <Button variant="outline" size="sm" className="border-dashed">
          <PlusCircle />
          {title}
        </Button>
      </PopoverTrigger>
      <PopoverContent className="p-0 w-[200px]" align="start">
        <Command>
          <CommandInput placeholder={title} />
          <CommandList>
            <CommandEmpty>No results found.</CommandEmpty>
            <CommandGroup>
              {/* TODO: add filter options here */}
            </CommandGroup>
          </CommandList>
        </Command>
      </PopoverContent>
    </Popover>
  );
}

export function NoteFilters() {
  return (
    <div className="py-4 flex items-center justify-between">
      <div className="flex items-center gap-2">
        <Input size="sm" placeholder="Filter notes..." />
        <NoteFilter title="Tags" />
      </div>
      <div>
        <Button size="sm" variant="outline">
          <Settings2Icon />
          View the last week
        </Button>
      </div>
    </div>
  );
}
