import { cva, VariantProps } from 'class-variance-authority';

export const widgetCard = cva(
    'dark:bg-green-900 space-y-4 p-4 rounded-2xl '
);

export const widgetList = cva(
    'dark:bg-green-700 space-y-3 rounded-2xl px-4 py-3'
);

export type WidgetCardVariantProps = VariantProps<typeof widgetCard>;
export type WidgetListVariantProps = VariantProps<typeof widgetList>;
