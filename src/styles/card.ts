import { cva, VariantProps } from 'class-variance-authority';

export const card = cva(
    'flex p-4 rounded-2xl bg-green-900 cursor-pointer',
    {
        variants: {
            intent: {
                primary:
                    'hover:bg-zinc-500 dark:hover:bg-gray-600',
            },
            size: {
                sm: 'text-sm px-3 py-1',
                md: 'text-md px-4 py-2',
                lg: 'text-lg px-6 py-3'
            },
            rounded: {
                none: 'rounded-none',
                sm: 'rounded-sm',
                md: 'rounded-md',
                lg: 'rounded-2xl',
                full: 'rounded-full',
            },
        },
        defaultVariants: {
            intent: 'primary',
            size: 'lg',
            rounded: 'lg',
        },
    }
);

export type CardVariantProps = VariantProps<typeof card>;
