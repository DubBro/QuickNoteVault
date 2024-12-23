'use client';

import type { Value } from '@udecode/plate-common';
import { useCreateEditor } from '@/components/editor/use-create-editor';
import { Editor, EditorContainer } from '@/components/plate-ui/editor';

import { Plate } from '@udecode/plate-common/react';

import { DndProvider } from 'react-dnd';
import { HTML5Backend } from 'react-dnd-html5-backend';

export interface PlateEditorProps {
  value?: Value;
  onValueChange?: (value: Value) => void;
}

export function PlateEditor({
  ...props
}: PlateEditorProps) {
  const editor = useCreateEditor(props);

  return (
    <DndProvider backend={HTML5Backend}>
      <Plate editor={editor}>
        <EditorContainer data-registry="plate">
          <Editor />
        </EditorContainer>
      </Plate>
    </DndProvider>
  );
}
