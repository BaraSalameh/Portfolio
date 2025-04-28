import { cva, VariantProps } from 'class-variance-authority';

export const button = cva(
    'truncate gap-2 flex items-center justify-center justify-between text-sm sm:text-base h-10 sm:h-12 px-4 sm:px-5 sm:w-auto font-medium transition-colors focus:outline-none focus:ring-2 focus:ring-offset-2 border hover:border-transparent text-sm sm:text-base h-10 sm:h-12 px-4 sm:px-5 sm:w-auto cursor-pointer',
    {
        variants: {
            intent: {
                primary:
                    'text-green-900 border-green-900 hover:bg-green-50 dark:text-green-300 dark:border-green-900 dark:hover:bg-green-900/50',
                secondary:
                    ' border-gray-400 hover:bg-gray-100 dark:text-gray-100 dark:border-gray-500 dark:hover:bg-gray-800/40',
                danger:
                    'text-red-700 border-red-600 hover:bg-red-50 dark:text-red-400 dark:border-red-500 dark:hover:bg-red-900/10',
                standard:
                    'bg-foreground text-background hover:bg-[#383838] dark:hover:bg-[#ccc]',
                basic:
                    'bg-transparent border-0 focus:ring-2 focus:ring-offset-0'
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
                full: 'rounded-full',
            },
        },
        defaultVariants: {
            intent: 'primary',
            size: 'md',
            rounded: 'md',
        },
    }
);

export type ButtonVariantProps = VariantProps<typeof button>;
