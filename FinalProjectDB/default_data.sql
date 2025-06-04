USE WorkspaceReservations;

-- Insert sample users, Password: 123456 for all users
INSERT INTO users (email, username, password, full_name) VALUES
('ivan.petrov@company.com', 'ivan.petrov', '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', 'Ivan Petrov'),
('maria.georgieva@company.com', 'maria.georgieva', '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', 'Maria Georgieva'),
('stoyan.dimitrov@company.com', 'stoyan.dimitrov', '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', 'Stoyan Dimitrov'),
('elena.ivanova@company.com', 'elena.ivanova', '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', 'Elena Ivanova'),
('petar.todorov@company.com', 'petar.todorov', '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', 'Petar Todorov');

-- Insert sample workspaces
INSERT INTO workspaces (name, floor, zone, has_monitor, has_docking_station, is_near_window, is_near_printer) VALUES
-- Floor 1
('Desk A1', 1, 'North', 1, 1, 1, 0),
('Desk A2', 1, 'North', 1, 1, 1, 0),
('Desk A3', 1, 'North', 1, 0, 0, 1),
('Desk B1', 1, 'South', 1, 1, 0, 1),
('Desk B2', 1, 'South', 1, 1, 0, 1),
('Desk B3', 1, 'South', 1, 0, 1, 0),
-- Floor 2
('Desk C1', 2, 'East', 1, 1, 1, 1),
('Desk C2', 2, 'East', 1, 1, 1, 0),
('Desk C3', 2, 'East', 1, 0, 0, 1),
('Desk D1', 2, 'West', 1, 1, 0, 1),
('Desk D2', 2, 'West', 1, 1, 0, 0),
('Desk D3', 2, 'West', 1, 0, 1, 1),
-- Floor 3
('Desk E1', 3, 'Central', 1, 1, 1, 1),
('Desk E2', 3, 'Central', 1, 1, 1, 0),
('Desk E3', 3, 'Central', 1, 0, 0, 1),
('Desk E4', 3, 'Central', 1, 1, 0, 1);