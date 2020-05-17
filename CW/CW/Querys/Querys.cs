/*
CREATE PROCEDURE PromoteStudents @Studies VARCHAR(100), @Semester INT
AS
BEGIN
	SET XACT_ABORT ON;
	BEGIN TRAN
	DECLARE @IdStudies INT = (SELECT IdStudy FROM Studies WHERE Name=@Studies);
	IF @IdStudies IS NULL
	BEGIN
		RAISERROR(11000, -1, -1, 'Studies not found');
RETURN;
	END;

	DECLARE @OldIdEnrollment INT = (SELECT IdEnrollment FROM Enrollment
									WHERE IdStudy = @IdStudies AND Semester = @Semester);

	IF @OldIdEnrollment IS NULL
	BEGIN
		RAISERROR(11000, -1, -1, 'No such enrollment');
RETURN
END;

DECLARE @IdEnrollment INT = (SELECT IdEnrollment FROM Enrollment
								WHERE IdStudy = @IdStudies AND Semester = @Semester + 1);
	IF @IdEnrollment IS NULL
	BEGIN
		DECLARE @NextIdEnrollment INT = (SELECT MAX(IdEnrollment) FROM Enrollment) + 1;
		INSERT INTO Enrollment VALUES(@NextIdEnrollment, @Semester + 1, @IdStudies, GETDATE());
SET @IdEnrollment = @NextIdEnrollment;

END;	

	UPDATE Student SET IdEnrollment = @IdEnrollment WHERE IdEnrollment = @OldIdEnrollment;

COMMIT
END;

*/