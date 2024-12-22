import { HomeIcon, PlusIcon, Trash2Icon, Users2Icon } from 'lucide-react';
import { Link, NavLink } from 'react-router';
import { Button } from './ui/button';
import { Sidebar, SidebarContent, SidebarGroup, SidebarGroupContent, SidebarMenu, SidebarMenuButton, SidebarMenuItem } from './ui/sidebar';

function AppSidebar() {
  return (
    <Sidebar collapsible="icon">
      <div className="p-2">
        <Button asChild className="flex w-full group-data-[collapsible=icon]:!size-8 group-data-[collapsible=icon]:!p-2 [&>span:last-child]:truncate [&>svg]:size-4 [&>svg]:shrink-0 transition-[width,height,padding] overflow-hidden" size="sm">
          <Link to="/notes/new">
            <PlusIcon />
            <span className="group-data-[collapsible=icon]:hidden">
              Add Note
            </span>
          </Link>
        </Button>
      </div>
      <SidebarContent>
        <SidebarGroup>
          <SidebarGroupContent>
            <SidebarMenu>
              <SidebarMenuItem>
                <SidebarMenuButton asChild>
                  <NavLink to="/">
                    <HomeIcon />
                    <span>Home</span>
                  </NavLink>
                </SidebarMenuButton>
              </SidebarMenuItem>
              <SidebarMenuItem>
                <SidebarMenuButton>
                  <Users2Icon />
                  <span>
                    Share with others
                  </span>
                </SidebarMenuButton>
              </SidebarMenuItem>
              <SidebarMenuItem>
                <SidebarMenuButton>
                  <Trash2Icon />
                  <span>
                    Trash
                  </span>
                </SidebarMenuButton>
              </SidebarMenuItem>
            </SidebarMenu>
          </SidebarGroupContent>
        </SidebarGroup>
      </SidebarContent>
    </Sidebar>
  );
}

export default AppSidebar;
