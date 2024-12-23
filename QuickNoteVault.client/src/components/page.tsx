import { cn } from '@/lib/utils';
import { forwardRef } from 'react';
import { Separator } from './ui/separator';
import { SidebarTrigger } from './ui/sidebar';

const Page = forwardRef<HTMLDivElement, React.ComponentPropsWithoutRef<'div'>>(({ ...props }, ref) => {
  return (
    <div ref={ref} {...props} className={cn('flex-1', props.className)} />
  );
});

Page.displayName = 'Page';

const PageHeader = forwardRef<HTMLDivElement, React.ComponentPropsWithoutRef<'header'>>(({ ...props }, ref) => {
  return (
    <header ref={ref} {...props} className={cn('p-4 bg-background', props.className)} />
  );
});

PageHeader.displayName = 'PageHeader';

const PageHeaderContent = forwardRef<HTMLDivElement, React.ComponentPropsWithoutRef<'div'>>(({ ...props }, ref) => {
  return (
    <div ref={ref} {...props} className={cn('flex items-center gap-2', props.className)} />
  );
});

PageHeaderContent.displayName = 'PageHeaderContent';

function PageSidebarTrigger({ ...props }: React.ComponentPropsWithoutRef<typeof SidebarTrigger>) {
  return (
    <div className="flex items-center">
      <SidebarTrigger {...props} className={cn('text-sidebar-foreground', props.className)} />
      <Separator orientation="vertical" className="h-4 mx-2" />
    </div>
  );
}

const PageContent = forwardRef<HTMLDivElement, React.ComponentPropsWithoutRef<'main'>>(({ ...props }, ref) => {
  return (
    <main ref={ref} {...props} className={cn('px-4', props.className)} />
  );
});

PageContent.displayName = 'PageContent';

export { Page, PageContent, PageHeader, PageHeaderContent, PageSidebarTrigger };
