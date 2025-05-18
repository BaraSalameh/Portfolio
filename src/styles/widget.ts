import { cva, VariantProps } from 'class-variance-authority';

export const widgetCard = cva(
    'relative dark:bg-green-900 p-4 rounded-2xl',
    {
        variants: {
            scroll: {
                true: 'max-h-[70vh] overflow-auto scrollbar-hide',
                false: null
            }
        },
        defaultVariants: {
            scroll: false
        }
    }
    
);

export const widgetList = cva(
    'dark:bg-green-700 space-y-3 rounded-2xl px-4 py-3', 
    {
        variants: {
            clickable : {
                true: 'cursor-pointer',
                false: null
            }
        },
        defaultVariants: {
            clickable: true
        }
    }
);

export type WidgetCardVariantProps = VariantProps<typeof widgetCard>;
export type WidgetListVariantProps = VariantProps<typeof widgetList>;
