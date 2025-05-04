import { PieChart, Pie, Cell, Tooltip, BarChart, Bar, XAxis, YAxis, ResponsiveContainer } from 'recharts';
import { GraduationCap, Clock } from 'lucide-react';
import dayjs from 'dayjs';
import { Paragraph } from './Paragraph';
import ResponsiveIcon from './ResponsiveIcon';
import { Header } from '../shared/Header';
import { Main } from '../shared/Main';
import { List } from './List';
import { CustomTooltip } from '../utils/customTooltip';

const COLORS = ['#10B981', '#3B82F6', '#F59E0B', '#EF4444', '#6366F1'];

export function EducationCard({ lstEducations }: {lstEducations: any}) {

    if (!Array.isArray(lstEducations) || lstEducations.length === 0) return null;

    // Pie data: Degrees distribution
    const degreeCounts = lstEducations.reduce((acc, edu) => {
        acc[edu.degree] = (acc[edu.degree] || 0) + 1;
        return acc;
    }, {});
    
    const degreeData = Object.entries(degreeCounts).map(([name, value]) => ({ name, value }));

    // Bar data: Duration per degree
    const durationData = lstEducations.map(edu => {
        const start = dayjs(edu.startDate);
        const end = edu.endDate ? dayjs(edu.endDate) : dayjs();
        return {
            name: edu.degree,
            duration: end.diff(start, 'month'),
        };
    });

    const colorMap = degreeData.reduce((acc, item, index) => {
        acc[item.name] = COLORS[index % COLORS.length];
        return acc;
    }, {} as Record<string, string>);

  return (
    <section className="bg-green-900 p-4 rounded-2xl">
        <Header paddingX='xs' paddingY='xs' space='sm'>
            <ResponsiveIcon icon={GraduationCap} />
            <Paragraph size='lg'> Education</Paragraph>
        </Header>
        <Main paddingX='none' paddingY='md'>
            {/* Charts Row */}
            <div className="grid grid-cols-1 sm:grid-cols-3">
                {/* Pie Chart: Degrees */}
                <div className="h-64">
                    <Paragraph size='md' position='center'>Degrees Overview</Paragraph>
                    <ResponsiveContainer >
                        <PieChart>
                            <Pie
                                data={degreeData}
                                dataKey="value"
                                nameKey="name"
                                cx="50%"
                                cy="50%"
                                outerRadius={50}
                                label
                            >
                                {degreeData.map((entry, index) => (
                                    <Cell key={`cell-${index}`} fill={colorMap[entry.name]} />
                                ))}
                            </Pie>
                            <Tooltip content={CustomTooltip} />
                        </PieChart>
                    </ResponsiveContainer>
                </div>

                {/* Bar Chart: Duration */}
                <div className="h-64 col-span-2 space-y-3">
                    <Paragraph size='md' position='center'>Duration (in months)</Paragraph>
                    <ResponsiveContainer width="100%" height="100%">
                        <BarChart data={durationData}>
                            <XAxis dataKey="name" />
                            <YAxis />
                            <Tooltip content={CustomTooltip} />
                            <Bar
                                    dataKey="duration"
                                    radius={[10, 0, 10, 0]}
                                    fillOpacity={1}
                                >
                                    {durationData.map((entry, index) => (
                                        <Cell key={`cell-bar-${index}`} fill={colorMap[entry.name]} />
                                    ))}
                                </Bar>
                        </BarChart>
                    </ResponsiveContainer>
                </div>
            </div>
        </Main>
        <Main paddingX='none' paddingY='none' >
            <List size='md' as='none' className='w-full'>
                {lstEducations.map(edu => (
                    <li key={edu.id} className="bg-green-700 p-4 rounded-xl shadow-sm space-y-2">
                        <Paragraph size='lg'>{edu.degree} in {edu.fieldOfStudy}</Paragraph>
                        <Paragraph>{edu.institution}</Paragraph>
                        <Paragraph className='gap-3 text-green-200'>
                            <ResponsiveIcon icon={Clock}/>
                            {dayjs(edu.startDate).format('MMM YYYY')} – {edu.endDate ? dayjs(edu.endDate).format('MMM YYYY') : 'Present'}
                        </Paragraph>
                    </li>
                ))}
            </List>
        </Main>
    </section>
  );
}
