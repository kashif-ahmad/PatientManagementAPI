-- Insert Patients
INSERT INTO Patient ([Name], Age, Gender, ContactNumber, [Address])
VALUES
    ('BamnzpASaEuzVTz0vpiBvA==', 30, 'Male',   'L8KDc21nJcVkKqFFXYqNEA==', 'vl9s4nBphNnDSBOinBSA1A=='),
    ('htUQ3SI4zNXkYLAWYAu4YA==', 25, 'Female', 'fTIWsDu7PqyYQSqhPccSVg==', '3tE9LBaiAcEzQvz03ApSDg=='),
    ('pzQFhlAvxlbgxKc+VSB5tw==', 29, 'Female', 'qoTAfnEaAIrU+g/zZY1iwg==', 'M9NR3nLDNzvIZS/BsVUtbQ==');

-- Insert Doctors
INSERT INTO Doctor ([Name], Specialization, Phone, Email)
VALUES
    ('Kane Mathews', 'Cardiology', 'oDxgphunUMyRGNZoM/1AEA==,', 'iM4EExq1Bodc3hnPJz6KrRte0YktYp9W1YVe63Cpj5w='),
    ('Jack Doe', 'Dermatology', '+YhzLV2JJS44Ujyo/YJSzw==', 'qcihDY0xJ4ql6Oebx1gkbct1za3iM5bAxF8an/JH0dY='),
    ('Swiss Kan', 'Radiology', 'ywzArplIVzWZFnwVrv8+Rg==', 'WzPcwCkKZbfnVSzMrgSMQteLgqR8VBczdYLANGUjB4E=');

-- Insert Users
INSERT INTO [User] (Username, PasswordHash, [Role])
VALUES
    ('admin', 'JAvlGPq9JyTdtvBO6x2llnRI1+gxwIyPqCKAn3THIKk=', 'Admin'),
    ('doctoruser', '75K3eLr+dx6JJFuJ7LwIpEpOFmwGZZkRiB84PURz6U8=', 'Doctor');