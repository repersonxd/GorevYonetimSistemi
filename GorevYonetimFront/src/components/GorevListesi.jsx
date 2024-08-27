
import { useTasks } from '../contexts/TaskContext';
import { List, Button, Spin, message } from 'antd';

function GorevListesi() {
    const { tasks, loading, error, deleteTask, fetchTasks } = useTasks();

    (() => {
        fetchTasks(); // G�revleri al
    }, [fetchTasks]);

    (() => {
        if (error) {
            message.error('G�revler al�n�rken bir hata olu�tu.');
        }
    }, [error]);

    if (loading) {
        return <div style={{ textAlign: 'center', marginTop: 50 }}><Spin size="large" /></div>;
    }

    return (
        <div>
            <List
                dataSource={tasks}
                renderItem={(task) => (
                    <List.Item
                        key={task.id}  // Buraya key prop'u eklendi
                        actions={[
                            <Button
                                key={`delete-${task.id}`} // Buraya da key prop'u eklendi
                                onClick={() => deleteTask(task.id)}
                                type="primary"
                                danger
                            >
                                Sil
                            </Button>,
                        ]}
                    >
                        {task.title}
                    </List.Item>
                )}
            />
        </div>
    );
}

export default GorevListesi;
