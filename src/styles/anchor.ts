import { cva, VariantProps } from 'class-variance-authority';

export const anchor = cva(
    'flex items-center gap-2 hover:underline hover:underline-offset-4',
    {
        variants: {
            size: {
                xs: 'text-xs px-2 py-1 sm:text-sm',
                sm: 'text-sm px-3 py-1',
                md: 'text-md px-4 py-2 mx-auto',
                lg: 'text-lg px-6 py-3 mx-auto'
            }
        },
        defaultVariants: {
            size: 'md'
        },
    }
);

export type AnchorVariantProps = VariantProps<typeof anchor>;
