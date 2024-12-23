import type { VariantProps } from 'class-variance-authority';
import { cn } from '@/lib/utils';
import { cva } from 'class-variance-authority';

import * as React from 'react';

const inputVariants = cva('flex w-full rounded-md border border-input bg-background px-3 py-2 text-base ring-offset-background file:border-0 file:bg-transparent file:text-sm file:font-medium file:text-foreground placeholder:text-muted-foreground focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50 md:text-sm', {
  variants: {
    size: {
      sm: 'h-9',
      default: 'h-10',
    },
  },
  defaultVariants: {
    size: 'default',
  },
});

export interface InputProps extends Omit<React.ComponentPropsWithoutRef<'input'>, 'size'>, VariantProps<typeof inputVariants> {}

const Input = React.forwardRef<HTMLInputElement, InputProps>(
  ({ className, type, size, ...props }, ref) => {
    return (
      <input
        type={type}
        className={cn(inputVariants({ className, size }))}
        ref={ref}
        {...props}
      />
    );
  },
);
Input.displayName = 'Input';

export { Input };
