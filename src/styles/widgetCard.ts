import { cva, VariantProps } from 'class-variance-authority';

export const widgetCard = cva(
    'p-4 rounded-2xl ',
    {
        variants: {
            intent: {
                primary: 'dark:bg-green-900 space-y-4',
                list: 'dark:bg-green-700 space-y-2 dark:hover:bg-gray-600 cursor-pointer'
            },
        },
        defaultVariants: {
            intent: 'primary',
        },
    }
);

export type WidgetCardVariantProps = VariantProps<typeof widgetCard>;
