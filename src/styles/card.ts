import { cva, VariantProps } from 'class-variance-authority';

export const card = cva(
    'flex rounded-2xl bg-green-900 cursor-pointer',
    {
        variants: {
            intent: {
                primary:
                    'hover:bg-zinc-500 dark:hover:bg-gray-600',
            },
            size: {
                sm: 'sm:px-3 sm:py-1 px-2 py-1',
                md: 'sm:px-4 sm:py-2 px-3 py-1',
                lg: 'sm:px-6 sm:py-3 px-5 py-2'
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
