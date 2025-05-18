import { cva, VariantProps } from 'class-variance-authority';

export const paragraph = cva(
    'flex items-center font-[family-name:var(--font-geist-mono)]',
    {
        variants: {
            intent: {
                primary:
                    'text-green-900 dark:text-green-900',
                secondary:
                    'text-gray-900 dark:text-gray-100',
                danger:
                    'text-red-700 dark:text-red-400',
                standard:
                    '',
            },
            size: {
                xs: 'text-xs',
                sm: 'text-xs sm:text-xs md: text-sm',
                md: 'text-sm sm:text-md md:text-md',
                lg: 'text-sm sm:text-md md:text-lg',
                xl: 'text-md sm:text-lg md:text-xl'
            },
            text: {
                standard: null,
                justify: 'text-justify',
            },
            position: {
                start: null,
                center: 'justify-center'
            },
            space: {
                none: 'gap-0',
                xs: 'gap-3',
                sm: 'gap-5',
                md: 'gap-10',
                lg: 'gap-15'
            },
        },
        defaultVariants: {
            intent: 'standard',
            size: 'md',
            text: 'standard',
            position: 'start',
            space: 'sm'
        },
    }
);

export type ParagraphVariantProps = VariantProps<typeof paragraph>;
