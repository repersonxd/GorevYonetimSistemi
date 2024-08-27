
import { useParams, useNavigate } from 'react-router-dom';
import { useTasks } from '../contexts/TaskContext';
import { Form, Input, Button, message, Spin } from 'antd';

function GorevDuzenle() {
    const { id } = useParams();
    const navigate = useNavigate();
    const { tasks, updateTask, loading } = useTasks();
    const [form] = Form.useForm();
    const [task, setTask] = (null);

    (() => {
        const taskToEdit = tasks.find(t => t.id === id);
        if (taskToEdit) {
            form.setFieldsValue(taskToEdit);
            setTask(taskToEdit);
        }
    }, [id, tasks, form]);

    const onFinish = async (values) => {
        try {
            await updateTask({ ...task, ...values });
            message.success('G�rev ba�ar�yla g�ncellendi.');
            navigate('/');
        } catch  {
            message.error('G�rev g�ncellenirken bir hata olu�tu.');
        }
    };

    if (!task) {
        return <Spin size="large" />;
    }

    return (
        <div>
            <Form form={form} onFinish={onFinish}>
                <Form.Item name="title" label="Ba�l�k" rules={[{ required: true, message: 'Ba�l�k gereklidir' }]}>
                    <Input />
                </Form.Item>
                <Form.Item>
                    <Button type="primary" htmlType="submit" loading={loading}>G�ncelle</Button>
                </Form.Item>
            </Form>
        </div>
    );
}

export default GorevDuzenle;
