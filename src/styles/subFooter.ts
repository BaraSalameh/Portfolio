import { cva, VariantProps } from 'class-variance-authority';

export const subFooter = cva(
    'row-start-3 flex flex-wrap items-center justify-center sm:gap-7'
);

export type SubFooterVariantProps = VariantProps<typeof subFooter>;
